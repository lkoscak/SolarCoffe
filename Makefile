PROJECT_NAME ?= SolarCoffe
ORG_NAME ?= SolarCoffe
REPO_NAME ?= SolarCoffe

.PHONY: migration db

migration:
	dotnet ef migrations add $(migration_name)

db:
	dotnet ef database update