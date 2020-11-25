# Project Variables
PROJECT_NAME ?= Stock
ORG_NAME ?= Stock
REPO_NAME ?= Stock

.PHONY: mig db 

mig:
	cd ./Data && dotnet ef --startup-project ../Api/ migrations add $(name) && cd ..

db:
	cd ./Data && dotnet ef --startup-project ../Api/ database update && cd ..