#!/bin/bash

# import.sh

mongoimport --host mongohost --db mongodb --collection names --type json --file /seed_data/name_data.json --jsonArray
mongoimport --host mongohost --db mongodb --collection admin --type json --file /seed_data/admin_data.json --jsonArray
