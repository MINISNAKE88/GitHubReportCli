# GitHub Quick Reporter CLI

A command-line tool written in C# using .NET 8 and the official Octokit library to retrieve and display GitHub user profile metrics.

For a general overview and use cases, visit the [GitHub Pages site](https://minisnake88.github.io/GitHubReportCli/).

## Features

* **Top 5 Popular Repositories:** Lists public repositories sorted by star count and includes fork metrics.
* **Language Breakdown:** Groups repositories to show the user's five most frequently used languages.
* **Review Counter:** Counts open pull requests where the user has been requested as a reviewer.

## Review Counter Implementation

To determine the number of pending reviews, the tool uses GitHub's search API with the following query:

`type:pr state:open review-requested:username`

This avoids downloading repository data and filtering pull requests on the client side, allowing the count to be obtained with a single API request.

## How to Run Locally

### Prerequisites

* .NET 8.0 SDK or later.

### Steps

```bash id="8hcyw3"
git clone https://github.com/MINISNAKE88/GitHubReportCli.git
```
