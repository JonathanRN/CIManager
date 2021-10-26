# CIManager

A Continuous Integration tool to easily integrate pipelines into applications.

## Supported Repository Managers & Platforms

| Repository Manager  | Platform |
| ------------- | ------------- |
| GitLab  | Unity<br/>Unity Personal |

## Usage

This project is compiled as a single file application and it must be ran from a terminal.

* `init` Starts the setup process.
    * You can also add a repository manager and a platform into the command. For example `init gitlab unity`.

You can add the flag `-h|--help` after any command to show more information. For example the command `./cim.exe init gitlab unity -h` outputs:

```txt
Setups a pipeline for Unity projects.

Usage: cim.exe init gitlab unity [options]

Options:

  -u | --use-defaults
  Automatically use default settings?
```
