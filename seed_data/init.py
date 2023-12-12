import json
import requests

with open('user_data.json', 'r') as file:
    data = json.load(file)

for entry in data:
    api_url = 'http://localhost:5000/users'
    response = requests.post(api_url, json=entry)

    if response.status_code == 201:
        print(f"POST request successful for entry: {entry}")
    else:
        print(f"POST request failed for entry: {entry}, Status Code: {response.status_code}")