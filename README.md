# Football History

An application for reviewing historical English Football League data. It is built in React-Typescript and C#, using an Azure-hosted SQL database and uses Github Actions for CI/CD.

The app contains data from the original 1888-1889 season through to the end of the last completed season. Data that can be found in the app includes league tables, match results, historical finishing positions and other information.

This app is a continual work in progress and mostly used as an experiment/practice engineering repo so is liable to substantial changes over time. The data within is not guaranteed to be 100% accurate, though this should improve over time.

Suggestions for improvements (either technical or feature) are welcome!

## Build/Deployment Status

| Football History                                                                         | Build Status                                                                                                                                                                                                                        |
| ---------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **[UI](https://football-history.azurewebsites.net/ "Football History")**                 | [![Build and deploy UI](https://github.com/yarhamjohn/football-history/actions/workflows/main_football-history-ui.yml/badge.svg)](https://github.com/yarhamjohn/football-history/actions/workflows/main_football-history-ui.yml)    |
| **[API](https://football-history-api.azurewebsites.net/swagger "Football History API")** | [![Build and deploy Api](https://github.com/yarhamjohn/football-history/actions/workflows/main_football-history-api.yml/badge.svg)](https://github.com/yarhamjohn/football-history/actions/workflows/main_football-history-api.yml) |

## Possible future work

- Add more metrics (e.g. trophies, records)
- Add additional analyses such as head-to-head matches
- Extend to include cup data and other leagues
- Include more detailed match-by-match data
- Technical enhancements, particularly around the front-end (e.g. RTL and Storybook)
