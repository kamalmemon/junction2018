#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Nov 24 14:28:26 2018

@author: daniyalusmani
"""

import pandas as pd
from fbprophet import Prophet
import matplotlib.pyplot as plt
import numpy as np
import datetime;
import pickle;

df = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_20180109.txt')
df1 = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_201801.txt')
df2 = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_201802.txt')
df3 = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_201803.txt')
df4 = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_201804.txt')
df5 = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_201805.txt')
df6 = pd.read_csv('../activity/Uusimaa_activity_data_hourly_20_min_break_MTC_201806.txt')
frames = [df, df1, df2, df3, df4, df5, df6]
df = pd.concat(frames)
df.head()
# print('number of dominant zones in sample', len(df.dominant_zone.unique()))
def getPrediction(zoneNum = 10):

    prediction = df[df['dominant_zone']== zoneNum]
    
    prediction["Datetime"] = pd.to_datetime(prediction["time"],format='%d.%m.%Y %H.%M.%S')
    
    prediction['hour'] = prediction.Datetime.dt.hour
    
    # Calculate average hourly fraction
    hourly_frac = prediction.groupby(['hour']).mean()/np.sum(prediction.groupby(['hour']).mean())
    hourly_frac.drop(['dominant_zone'], axis = 1, inplace = True)
    hourly_frac.columns = ['fraction']
    
    # convert to time series from dataframe
    prediction.index = prediction.Datetime
    prediction.drop(['dominant_zone','hour','Datetime'], axis = 1, inplace = True)
    
    daily_train = prediction.resample('H').sum()
    
    daily_train['ds'] = daily_train.index
    daily_train['y'] = daily_train["count"]
    daily_train.drop(['count'],axis = 1, inplace = True)
    
    m = Prophet(daily_seasonality=True, seasonality_prior_scale=0.1)
    m.fit(daily_train)
    future = m.make_future_dataframe(periods=1000, freq='H')
    forecast = m.predict(future)
    forecast.daily = forecast.daily.clip(0)
    forecast.weekly = forecast.weekly.clip(0)
    forecast.yhat = forecast.yhat.clip(0)
    # fig1 = m.plot(forecast)
    
    # components of forecast, yearly, weekly
    # m.plot_components(forecast)
    return forecast
# ret = load_obj('forecast-1000')

