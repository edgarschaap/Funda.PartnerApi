# PartnerApi.Client

This project is just for illustration purposes only. It is basically a SDK that exposes the Partner Api functionality. 
Usually it could be developed alongside the Partner Api and published as a separate package and then consumed by 
any other solution that needs its functionality.

# Purpose

The client takes care of several things so this doesn't clutter the solutions using it. 

* Http client is created and default headers such as correlation id or application calling the client
  are attached to every request
* Resilience in the form of retries using Polly
* Results are handled based on Http status codes (using result pattern)
* Small safety check that requires confirming that result is successful before accessing the value

# Usage

`ClientPartnerApiServiceFactory` should be instantiated with apis base url. Depending on the api structure, a
`Service` represents a group of endpoints. Each service can be created using the factory. `Action` describes the 
behaviour of a single endpoint.
