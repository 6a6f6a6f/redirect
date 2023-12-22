# Redirect

This is the `redirect` tool, which you can do in `curl`, but here you have the fancy output (and the bugs).

```text
__ _  _| o __ _  _ _|_
 | (/_(_| | | (/_(_  |_

Usage:
  redirect <uri> [options]

Arguments:
  <uri>  Target URI to check for redirects

Options:
  -t, --timeout <count> (REQUIRED)       Timeout duration in seconds [default: 5]
  -h, --header <name:value>              Custom header to add to the request
  -m, --max-redirect <count> (REQUIRED)  Maximum number of redirects to follow [default: 5]
  -p, --proxy <host:port>                Proxy to use for the request
  -i, --ignore-certificate-errors        Ignore HTTPS-related certificate errors [default: True]
  --version                              Show version information
  -?, -h, --help                         Show help and usage information
```
