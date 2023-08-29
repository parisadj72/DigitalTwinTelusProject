from flask import *
import json
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
import time
from bs4 import BeautifulSoup
import requests
# from appium import webdriver
# desired_capabilities = {
#     'platformName': 'iOS',
#     'platformVersion': '16',
#     'deviceName': 'your_device_name',
#     # 'app': 'path/to/your/app',
#     # 'bundleId': 'com.example.browserapp',  # Replace with the browser app's bundle ID
# }
session = requests.Session()
app = Flask(__name__)

@app.route('/', methods = ['GET'])
def return_strength():

    # url = "https://66c7-2604-3d09-d7f-ec20-e1db-468-b06c-72a1.ngrok-free.app"
    headers = {
        # "User-Agent": "Your User Agent",  # Replace with your User-Agent header
        "ngrok-skip-browser-warning": "1999"  # Replace with any custom headers you want to include
    }

    a= True

    driver = webdriver.Chrome(keep_alive = True)
    url = "https://speedof.me/api/doc/sample1.html"
   
    response = driver.get("https://speedof.me/api/doc/sample1.html")
    # response = requests.get(url, headers=headers)


    button = driver.find_element(By.ID, "btnStart")
    button.click()
    time.sleep(25)
    page_source = driver.page_source
    htmlcontent = BeautifulSoup(page_source, "html.parser")


    parent_tag = htmlcontent.find("div", id = "msg")


    if parent_tag:
        nested = parent_tag.find('h4')
        nested = str(nested)
        print(nested)


    download = nested[14:20]
    upload = nested[38:45]
    print("download " + download)
    print("upload " + upload + "Mbps" )
    data = {"download": download , "upload": upload}
    json_dump = json.dumps(data)
    driver.quit()
    return json_dump
    


if __name__ == "__main__":
    app.run(port = 7777)








