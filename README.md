# Indoor Localization Backend — Docker Setup

## 1. Prerequisites
- Docker Desktop
- .NET 8 SDK (required only if you are working with migrations)

Check versions:

docker --version
dotnet --version


---

## 2. Clean old containers 

In the **root folder** of the project:

docker compose down -v


If the database was modified manually inside the container, remove the volume completely:

docker volume rm indoor-localization-backend_pgdata


Reset the entire application (removes everything):

docker compose down -v


---

## 3. `.env` file
You must create a `.env` file in the root directory with the following structure:

POSTGRES_USER=<postgres_user>
POSTGRES_PASSWORD=<postgres_password>
POSTGRES_DB=<db_name>

JWT_SECRET=<secret_with_minimum_32_characters>
`JWT_SECRET` must contain at least 32 characters (HS256 requirement).

---

## 4. Starting the Docker environment - after new REST paths are added or schema changes

### Clean old containers
docker compose down -v

Remove volumes, containers and images mannualy in docker desktop if needed.

### If database schema changed
dontet ef database update

### Build everything:

docker compose up --build


### Run in background:

docker compose up -d


### Stop all containers:

docker compose down


### View backend logs:

docker logs -f indoor_backend


---

## 5. Ports

### Backend:
- Container: **8080**
- Host: **5000**
- Swagger UI:  
  `http://localhost:5000/swagger`

### Database (Postgres):
- Host port: **5432**

Connect to the database:

docker exec -it indoor_postgres psql -U <POSTGRES_USER> -d <POSTGRES_DB>


List tables:

\dt


---

# DATABASE SCHEMA CHANGES (IMPORTANT)

Use Entity Framework Core migrations.

---

## 1. Generate a new migration

dotnet ef migrations add <MigrationName>


## 2. Commit & push the migration

## 3. Redeploy using Docker

docker compose down -v
docker compose up --build


Backend automatically executes:

db.Database.Migrate();

