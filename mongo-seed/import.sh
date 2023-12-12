#! /bin/bash

# Import data into the 'names' collection
mongoimport --host mongohost --db mongodb --collection names --type json --file /mongo-seed/name_data.json --jsonArray

