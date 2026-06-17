# GitHub Quick Reporter CLI

A command-line tool written in C# using .NET 8 and the official Octokit library to retrieve and display GitHub user profile metrics.

For a general overview and use cases, visit the [GitHub Pages site](https://minisnake88.github.io/GitHubReportCli/).

## Features

* **Top 5 Popular Repositories:** Lists public repositories sorted by star count and includes fork metrics.
* **Language Breakdown:** Groups repositories by language to show the user's five most frequently used languages.
* **Review Counter:** Counts open pull requests where the user has been requested as a reviewer.

## Review Counter Implementation

The review counter uses GitHub's search API with the following query:

`type:pr state:open review-requested:username`

This approach avoids downloading repository data and additional client-side filtering, reducing the operation to a single API request.

## How to Run Locally

### Prerequisites

* .NET 8.0 SDK or later.

### Steps

```bash
git clone https://github.com/MINISNAKE88/GitHubReportCli.git
```
