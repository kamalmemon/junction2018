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
    return jsonify(res.to_json()), 200

if __name__ == "__main__":
    #main()
    app.run(host='0.0.0.0', debug=True)
    