#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Nov 24 19:41:19 2018

@author: daniyalusmani
"""

from flask import Flask, jsonify
from flask_restful import Api, Resource, reqparse
from forecast import getPrediction
from flask_cors import CORS
import pandas as pd
from io import BytesIO
import base64
import json
import matplotlib.pyplot as plt

def create_app():
    # main()
    return Flask(__name__)

app = create_app()
cors = CORS(app)


@app.route('/')
def homepage():
    return 'Hello', 200


@app.route('/forecast/<int:grid>')
def get(grid):
    res = getPrediction(grid)
    res = res.daily[0:24]
    #return jsonify(res.loc[res.nonzero()].mean().to_json()), 200
    return json.dumps({'daily_mean': res.loc[res.nonzero()].mean(), 
           'plot_img': _get_plot(res)})

def _get_plot(forecast):
    forecast.plot.barh(figsize=(6,6))
    img = BytesIO()  # create the buffer
    plt.savefig(img, format='png')  # save figure to the buffer
    img.seek(0)  # rewind your buffer
    plot_data = base64.b64encode(img.read()).decode()
    return plot_data

if __name__ == "__main__":
    #main()
    app.run(host='0.0.0.0', debug=True)
    