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

Output example:

```text
Response Map:
└── Source URI: http://facebook.com/?param1=1&param2=2
    ├── Destination URI: https://facebook.com/?param1=1&param2=2
    ├── Response Time: 267.4055 ms
    ├── Status Code: 200 (OK)
    ├── Headers: 
    │   ├── Vary: Accept-Encoding
    │   ├── reporting-endpoints: default="https://www.facebook.com/ajax/browser_error_reports/?device_level=unknown"
    │   ├── report-to: {"max_age":259200,"endpoints":[
    │   │   {"url":"https:\/\/www.facebook.com\/ajax\/browser_error_reports\/?device_level=unknown"}]}
    │   ├── Content-Security-Policy: default-src data: blob: 'self' https://*.fbsbx.com 'unsafe-inline' *.facebook.com *.fbcdn.net 
    │   │   'unsafe-eval';script-src *.facebook.com *.fbcdn.net *.facebook.net *.google-analytics.com *.google.com 127.0.0.1:* 
    │   │   'unsafe-inline' blob: data: 'self' connect.facebook.net 'unsafe-eval';style-src fonts.googleapis.com *.fbcdn.net data: 
    │   │   *.facebook.com 'unsafe-inline';connect-src *.facebook.com facebook.com *.fbcdn.net *.facebook.net wss://*.facebook.com:* 
    │   │   wss://*.whatsapp.com:* wss://*.fbcdn.net attachment.fbsbx.com ws://localhost:* blob: *.cdninstagram.com 'self' 
    │   │   http://localhost:3103 wss://gateway.facebook.com wss://edge-chat.facebook.com wss://snaptu-d.facebook.com 
    │   │   wss://kaios-d.facebook.com/ v.whatsapp.net *.fbsbx.com *.fb.com;font-src data: *.gstatic.com *.facebook.com *.fbcdn.net 
    │   │   *.fbsbx.com;img-src *.fbcdn.net *.facebook.com data: https://*.fbsbx.com *.tenor.co media.tenor.com facebook.com 
    │   │   *.cdninstagram.com fbsbx.com fbcdn.net *.giphy.com connect.facebook.net *.carriersignal.info blob: 
    │   │   android-webview-video-poster: googleads.g.doubleclick.net www.googleadservices.com *.whatsapp.net *.fb.com 
    │   │   *.oculuscdn.com;media-src *.cdninstagram.com blob: *.fbcdn.net *.fbsbx.com www.facebook.com *.facebook.com 
    │   │   https://*.giphy.com data:;frame-src *.doubleclick.net *.google.com *.facebook.com www.googleadservices.com *.fbsbx.com 
    │   │   fbsbx.com data: www.instagram.com *.fbcdn.net https://paywithmybank.com https://sandbox.paywithmybank.com;worker-src blob: 
    │   │   *.facebook.com data:;block-all-mixed-content;upgrade-insecure-requests;
    │   ├── document-policy: force-load-at-top
    │   ├── permissions-policy-report-only: autoplay=(), clipboard-read=(), clipboard-write=(), encrypted-media=(), fullscreen=(), 
    │   │   keyboard-map=()
    │   ├── permissions-policy: accelerometer=(), ambient-light-sensor=(), bluetooth=(), camera=(self), fullscreen=(self), gamepad=*, 
    │   │   geolocation=(self), gyroscope=(), hid=(), idle-detection=(), local-fonts=(), magnetometer=(), microphone=(self), midi=(), 
    │   │   otp-credentials=(), payment=(), picture-in-picture=(self), publickey-credentials-get=(self), screen-wake-lock=(), serial=(),
    │   │   usb=(), window-management=()
    │   ├── cross-origin-resource-policy: same-origin
    │   ├── cross-origin-opener-policy: same-origin-allow-popups
    │   ├── Pragma: no-cache
    │   ├── Cache-Control: no-store, must-revalidate, no-cache, private
    │   ├── X-Content-Type-Options: nosniff
    │   ├── X-XSS-Protection: 0
    │   ├── X-Frame-Options: DENY
    │   ├── Strict-Transport-Security: max-age=15552000; preload
    │   ├── X-FB-Debug: IGdO3ABawtOmPmzlpynpSXk6K6YLzJAguqqA4neRAKARIcpiuTeJatyDVAtKVZu3LoqePQJc2h0Nd1LeT+qXtQ==
    │   ├── Date: Fri, 22 Dec 2023 00:51:45 GMT
    │   ├── Transfer-Encoding: chunked
    │   ├── Alt-Svc: h3=":443"
    │   └── Connection: keep-alive
    └── Source URI: https://facebook.com/?param1=1&param2=2
        ├── Destination URI: https://www.facebook.com/?param1=1&param2=2
        ├── Response Time: 558.4414 ms
        ├── Status Code: 301 (MovedPermanently)
        ├── Headers: 
        │   ├── Location: https://www.facebook.com/?param1=1&param2=2
        │   ├── Strict-Transport-Security: max-age=15552000; preload
        │   ├── X-FB-Debug: 59o2syFxUm0YhKDFdWdr4kDYhKzD4njhc4bi8IDcmG+SdckvyowYj16W59ZssFifV/+ZZ81rLneg66dg0GZ+1A==
        │   ├── Date: Fri, 22 Dec 2023 00:51:44 GMT
        │   ├── Alt-Svc: h3=":443"
        │   └── Connection: keep-alive
        ├── Query Parameters: 
        │   ├── param1: 1
        │   └── param2: 2
        └── Source URI: https://www.facebook.com/?param1=1&param2=2
            ├── Destination URI: https://www.facebook.com/unsupportedbrowser
            ├── Response Time: 307.6376 ms
            ├── Status Code: 302 (Found)
            └── Headers: 
                ├── Location: https://www.facebook.com/unsupportedbrowser
                ├── Strict-Transport-Security: max-age=15552000; preload
                ├── X-FB-Debug: jXyC/8G7yJAYL71jwlTnKJSTg4k2JSgmCtEyUadq59hodHph6DTAKRjtHaciW3PCo0x2ts8z01a2KZ5A1Kme7w==
                ├── Date: Fri, 22 Dec 2023 00:51:45 GMT
                ├── Alt-Svc: h3=":443"
                └── Connection: keep-alive
```
