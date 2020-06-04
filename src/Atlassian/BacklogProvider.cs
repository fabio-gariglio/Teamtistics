using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using Domain;
using Microsoft.Extensions.Options;

namespace Atlassian
{
    public class BacklogProvider : IBacklogProvider
    {
        private readonly Jira.Jira _client;

        public BacklogProvider(IOptions<AtlassianOptions> options)
        {
            _client = Jira.Jira.CreateRestClient(
                options.Value.Url,
                options.Value.Username,
                options.Value.Password
            );
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string sprintName)
        {
            var issues = from i in _client.Issues.Queryable
                where i["Sprint"] == new LiteralMatch(sprintName) && i.Type != "Sub-Task"
                select i;

            var tasks = issues.ToArray().Select(AsItemAsync);

            await Task.WhenAll(tasks);

            return tasks.Select(t => t.Result).ToArray();
        }

        private async Task<Item> AsItemAsync(Issue issue) => new Item
        {
            Id = issue.Key.Value,
            Sprint = issue["Sprint"].Value,
            Status = issue.Status.Name,
            Title = issue.Summary,
            Type = issue.Type.Name,
            User = issue.AssigneeUser?.DisplayName,
            Estimate = GetEstimate(issue),
            Transitions = new StatusTransitionCollection(await GetTransitionsAsync(issue))
        };

        private static async Task<ICollection<StatusTransition>> GetTransitionsAsync(Issue issue) =>
            (await issue.GetChangeLogsAsync())
            .Where(IsStatusChange)
            .Select(AsStatusTransition)
            .ToList();

        private static bool IsStatusChange(IssueChangeLog changeLog) =>
            changeLog.Items.Any(i => i.FieldName == "status");

        private static StatusTransition AsStatusTransition(IssueChangeLog changeLog) => new StatusTransition
            {
                Date = changeLog.CreatedDate,
                User = changeLog.Author.DisplayName,
                From = changeLog.Items.Single().FromValue,
                To = changeLog.Items.Single().ToValue,
            };

        private static Estimate GetEstimate(Issue issue) =>
            IsSpike(issue)
                ? AsDurationEstimate(issue)
                : AsEffortEstimate(issue);

        private static bool IsSpike(Issue issue) =>
            issue.Type.Name == "Spike";

        private static Estimate AsDurationEstimate(Issue issue) => new DurationEstimate();

        private static Estimate AsEffortEstimate(Issue issue) => new EffortEstimate
        {
            Points = double.Parse(issue["Story Points"]?.Value ?? "0")
        };
    }
}
