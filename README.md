# Sendora City API

[![CI ⚙️](https://github.com/pchanvallon/sendoracity-api/actions/workflows/CI.yml/badge.svg)](https://github.com/pchanvallon/sendoracity-api/actions/workflows/CI.yml)
[![CD 🚀](https://github.com/pchanvallon/sendoracity-api/actions/workflows/CD.yml/badge.svg)](https://github.com/pchanvallon/sendoracity-api/actions/workflows/CD.yml)

Sample .NET Core API that creates city, houses and stores using a PostgresSQL database.

## Usage

### Requirements

Docker-compose should be installed in order to run the project.

### How to run

1. Clone the repository
2. Run `docker-compose up` in the root folder

### How to use

Open a brower and navigate to `http://localhost:8080/swagger/index.html` to see the API documentation.

### Endpoints

The API has 3 endpoints:

* `/api/cities`:
  * `GET /api/cities` - List cities
  * `POST /api/cities` - Creates a new city
  * `GET /api/cities/{id}` - Gets a city
  * `PATCH /api/cities/{id}` - Updates a city
  * `DELETE /api/cities/{id}` - Deletes a city
* `/api/houses`:
  * `GET /api/houses` - List houses
  * `POST /api/houses` - Creates a new house
  * `GET /api/houses/{id}` - Gets a house
  * `PATCH /api/houses/{id}` - Updates a house
  * `DELETE /api/houses/{id}` - Deletes a house
* `/api/stores`:
  * `GET /api/stores` - List stores
  * `POST /api/stores` - Creates a new store
  * `GET /api/stores/{id}` - Gets a store
  * `PATCH /api/stores/{id}` - Updates a store
  * `DELETE /api/stores/{id}` - Deletes a store
