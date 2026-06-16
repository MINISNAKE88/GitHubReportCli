using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace GitHubReportCli
{
    class Program
    {
        // NOTE: To avoid strict API rate limits, you can generate a Personal Access Token (Classic) 
        // on GitHub with minimal permissions and paste it here. Leaving it empty works for public use but with limits.
        private static readonly string GitHubToken = "";

        static async Task Main(string[] args)
        {
            Console.Title = "GitHub Quick Reporter";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=============================================");
            Console.WriteLine("         GITHUB QUICK REPORTER CLI           ");
            Console.WriteLine("=============================================\n");
            Console.ResetColor();

            Console.Write("Enter GitHub Username: ");
            string? username = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Username cannot be empty.");
                return;
            }

            // Initialize the Octokit client
            var client = new GitHubClient(new ProductHeaderValue("GitHubQuickReporterCli"));

            if (!string.IsNullOrEmpty(GitHubToken))
            {
                client.Credentials = new Credentials(GitHubToken);
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n[i] Fetching information from GitHub... Please wait.\n");
                Console.ResetColor();

                // Fetch all repositories for the specified user
                var repos = await client.Repository.GetAllForUser(username);

                // --- SECTION 1: Most Popular Repositories ---
                DisplayTopRepositories(repos);

                // --- SECTION 2: Top Languages Used ---
                DisplayTopLanguages(repos);

                // --- SECTION 3: Pending Pull Request Reviews ---
                await DisplayPendingReviewsCount(client, username);

            }
            catch (NotFoundException)
            {
                PrintError($"The user '{username}' was not found on GitHub.");
            }
            catch (AuthorizationException)
            {
                PrintError("Authorization error. Please check your Access Token if configured.");
            }
            catch (Exception ex)
            {
                PrintError($"An unexpected error occurred: {ex.Message}");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=============================================");
            Console.ResetColor();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DisplayTopRepositories(IReadOnlyList<Repository> repos)
        {
            var topRepos = repos
                .OrderByDescending(r => r.StargazersCount)
                .Take(5)
                .ToList();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("⭐ 1) TOP 5 MOST POPULAR REPOSITORIES:");
            Console.ResetColor();

            if (!topRepos.Any())
            {
                Console.WriteLine("   No public repositories found.");
                return;
            }

            foreach (var repo in topRepos)
            {
                Console.WriteLine($"   • {repo.Name,-25} | Stars: {repo.StargazersCount,-5} | 🍴 Forks: {repo.ForksCount}");
            }
            Console.WriteLine();
        }

        private static void DisplayTopLanguages(IReadOnlyList<Repository> repos)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("📊 2) MOST USED LANGUAGES SUMMARY (Based on repo count):");
            Console.ResetColor();

            // Group repositories by their primary language detected by GitHub
            var languageStats = repos
                .Where(r => !string.IsNullOrEmpty(r.Language))
                .GroupBy(r => r.Language)
                .Select(g => new { Language = g.Key, Count = g.Count() })
                .OrderByDescending(l => l.Count)
                .Take(5);

            if (!languageStats.Any())
            {
                Console.WriteLine("   Could not determine language statistics.");
                return;
            }

            foreach (var lang in languageStats)
            {
                Console.WriteLine($"   • {lang.Language,-25} | Used in {lang.Count} repo(s)");
            }
            Console.WriteLine();
        }

        private static async Task DisplayPendingReviewsCount(GitHubClient client, string username)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("🔍 3) PENDING PULL REQUEST REVIEWS:");
            Console.ResetColor();

            // FIX: Passing the search qualifiers directly into the constructor as a raw query string.
            // This bypasses structural type mismatches across different Octokit versions completely.
            var searchRequest = new SearchIssuesRequest($"type:pr state:open review-requested:{username}");

            var searchResult = await client.Search.SearchIssues(searchRequest);

            Console.Write("   You have ");
            Console.ForegroundColor = searchResult.TotalCount > 0 ? ConsoleColor.Yellow : ConsoleColor.Cyan;
            Console.Write(searchResult.TotalCount);
            Console.ResetColor();
            Console.WriteLine(" Pull Request(s) awaiting your code review.");
        }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Error: {message}");
            Console.ResetColor();
        }
    }
}