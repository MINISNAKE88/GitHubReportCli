# GitHub Quick Reporter CLI

A command-line interface tool written in C# using .NET 8 and the official Octokit library to generate instant GitHub user profile metrics.

## Features

* **Top 5 Popular Repositories:** Displays public repositories sorted by star count alongside fork metrics.
* **Language Breakdown:** Groups user repositories to identify their top 5 most utilized languages.
* **Optimized Review Counter:** Calculates the exact number of open Pull Requests awaiting the user's code review.

## Technical Optimization Highlight

Instead of manually downloading a user's entire repository history and filtering pull requests locally—which risks hitting GitHub's strict rate limits—this tool aggregates query criteria natively via the SearchIssuesRequest constructor string:

`type:pr state:open review-requested:username`

This optimizes performance down to a single, lightweight API call, demonstrating efficient and production-ready API consumption.

## How to Run Locally

### Prerequisites

* .NET 8.0 SDK or higher.

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/MINISNAKE88/GitHubReportCli.git
