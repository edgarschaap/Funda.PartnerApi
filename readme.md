# Funda Assessment

This repository contains the assessment for Funda. It uses the Funda
Partner Api to retrieve all objects searched by specific search keys.

# Structure

The project is structured using Ports and Adapters architecture, which
is also known as Hexagonal or Onion architecture. Another similar well
known structure is Clean architecture.

At the center of it lives the `domain` which contains the business logic.
`Adapters` plug into the `Ports` interfaces provided by the domain. These 
could be any kind of dependencies like api's, databases, blob storages or
simple in memory stores. `Hosts` are presentation or execution layers so basically 
the entry point of the solution and could represent api's, single page apps 
or console applications like implemented right now.
`Usecases` usually bind different `repositories` together and perform business
logic.
`ValueTypes` are immutable objects that represent various domain types and wrap 
primitives as strict types. They also help to avoid property displacement.

In memory representation helps with testing `usecase` logic without relying on
external dependencies. They also help from leaking adapter functionality into the
domain and keep the domain honest this way.

# PartnerApi Client

Please do read the documentation provided there as well =)

# Usage

- Find the `settings.yaml` in `Host.Console` project and add the auth token
- Execute `Host.Console` 
- When console window prompts for entry
  - Pick 1. for top 10 realtors for search key `Amsterdam`
  - Pick 2. for top 10 realtors for search key `Amsterdam | Tuin`

# Improvements

Obviously this is just a test application to show my capabilities, but
it doesnt mean that it is perfect. In the next iteration possible things
to improve would be

- Create and use Builder patterns to create test data and implement `A` class
- In the PartnerApi.Client i use a simple while loop to paginate through results.
  A better way would be returning an iterator and let the consuming application handle
  the fetching of next page
- Use WireMock to test different http response statuses
- I wish I started with a git repo, so you could see how I document my commits and steps 
  I took would be visible
- Please tell me what else could be better =)



