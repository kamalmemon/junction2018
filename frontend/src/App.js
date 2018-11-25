import React, { Component } from "react";
import axios from "axios";
import Map from "./Map";
import { GoogleApiWrapper } from "google-maps-react";
import Autocomplete from "react-google-autocomplete";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import TagsInput from "react-tagsinput";
import Analysis from "./Analysis";
import "react-tagsinput/react-tagsinput.css"; // If using WebPack and style-loader.
import "./App.css";

class App extends Component {
  state = {
    startDate: new Date(),
    markers: [],
    tags: [],
    polyCords: null,
    center: null,
    selectedView: "map",
    selectedGridId: null,
    gridPrediction: null,
    loading: false
  };

  async requestApi(lat, lng) {
    const getdata = await axios.get(
      `http://gravity02-dev.azurewebsites.net/api/events?lat=${lat}&lon=${lng}&range=${2000}`
    );
    this.setState({
      markers: getdata.data
    });
  }

  onChange = date => {
    this.setState({ startDate: date });
    this.rafayApi();
  };

  handleTagChange = tags => {
    this.setState({ tags: tags });
    this.rafayApi();
  };

  async rafayApi() {
    const { center, startDate, tags } = this.state;
    if (!center) {
      return;
    }
    const RafayApiData = await axios.get(
      `http://gravity02-dev.azurewebsites.net/api/events?lat=${
        center.lat
      }&lon=${
        center.lng
      }&range=${500}&eventDateTime=${startDate.toISOString()}&tags=${tags.join(
        ","
      )}`
    );
    if (RafayApiData.data) {
      this.setState({
        markers: RafayApiData.data
      });
    }
  }
  addTag(t) {
    if (this.state.tags.indexOf(t) < 0) {
      this.setState(ps => ({ tags: ps.tags.push(t) }));
    }
  }
  async analysisApi() {
    if (!this.state.selectedGridId) {
      return;
    }
    this.setState({ loading: true, gridPrediction: null });

    const apiData = await axios.get(
      `http://10.100.31.58:5000/forecast/${this.state.selectedGridId}`
    );
    this.setState({ gridPrediction: apiData.data, loading: false });
  }
  async polyCordsApi() {
    const { center } = this.state;
    this.setState({ loading: true, selectedGridId: null, polyCords: null });
    const apiData = await axios.get(
      `http://10.100.31.58:8888/grid_id?lat=${center.lat}&long=${center.lng}`
    );
    const polyCords = apiData.data.poly_cords;

    if (polyCords) {
      this.setState(
        {
          loading: false,
          selectedGridId: apiData.data.grid_id,
          polyCords: polyCords.map(p => ({ lat: p[0], lng: p[1] }))
        },
        () => {
          this.analysisApi();
        }
      );
    }
  }
  changeView(e) {
    this.setState({ selectedView: e.target.value });
  }

  render() {
    const inputDisabled = this.state.center;
    return (
      <div className="main-app-container">
        <h2>Actually Useful Digital Advertisement</h2>
        <br />
        <div className="input-container">
          <Autocomplete
            style={{ width: "35%" }}
            onPlaceSelected={place => {
              const { lat, lng } = place.geometry.location;
              this.setState({ center: { lat: lat(), lng: lng() } }, () => {
                this.rafayApi();
                this.polyCordsApi();
              });
            }}
            types={["(regions)"]}
            componentRestrictions={{ country: "fi" }}
          />
          <DatePicker
            selected={this.state.startDate}
            onChange={this.onChange}
          />
          <TagsInput value={this.state.tags} onChange={this.handleTagChange} />
          <select onChange={this.changeView.bind(this)}>
            <option defaultValue value="map">
              Map
            </option>
            <option value="analysis">Analysis</option>
          </select>
        </div>
        {this.state.selectedView == "map" && (
          <Map
            center={this.state.center}
            markers={this.state.markers}
            polyCords={this.state.polyCords}
            onMapClick={() => {}}
            estimatedUsers={
              this.state.gridPrediction
                ? this.state.gridPrediction.daily_mean
                : null
            }
            addTag={this.addTag.bind(this)}
            loading={this.state.loading}
          />
        )}

        {this.state.selectedView == "analysis" && (
          <Analysis
            graph={
              this.state.gridPrediction
                ? this.state.gridPrediction.plot_img
                : null
            }
            estimatedUsers={
              this.state.gridPrediction
                ? this.state.gridPrediction.daily_mean
                : null
            }
          />
        )}
      </div>
    );
  }
}

export default GoogleApiWrapper({
  apiKey: `AIzaSyCR7DA6hA3Bm5m9tpxeFP9eW_teXWD-qrw`,
  libraries: ["places", "visualization"]
})(App);
