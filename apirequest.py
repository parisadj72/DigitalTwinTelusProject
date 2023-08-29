import requests

# Replace this URL with the actual API endpoint you want to use
api_url = "http://127.0.0.1:7777"


response = requests.get(api_url)
data = response.json()
print(data)