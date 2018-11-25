import React, { Component } from "react";
import yhat from "./yhat.png";
import aggregate from "./aggregate.png";
class Analysis extends Component {
  render() {
    return (
      <div>
        {this.props.graph && (
          <div className="image-container">
            <img src={`data:image/png;base64, ${this.props.graph}`} />

            <p>Actual users predicted: {this.props.estimatedUsers}</p>
          </div>
        )}
        <div className="image-container">
          <img src={yhat} />

          <p>Timeseries forcasting</p>
        </div>
        <div className="image-container">
          <img src={aggregate} />

          <p>Granular forcast</p>
        </div>
      </div>
    );
  }
}

export default Analysis;
