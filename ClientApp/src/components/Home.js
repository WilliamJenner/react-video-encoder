import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Constants } from "../utils/constants";
import $ from "jquery";


export class Home extends Component {
  constructor() {
    super()

    this.state = {
      url: `${Constants.apiUrl}${Constants.videoControllerUrl}${Constants.setVideoUrl}`,
      videoInput: undefined,
    }
  }

  static displayName = Home.name;

  onVideoInputChange = (e) => {
    
    this.setState({ [e.target.name]: e.target.files[0] }, () => {
    })
  }

  onSubmit = (e) => {
    e.preventDefault();
    // get our form data out of state\
    var formData = this.createFormData();
    console.log(formData.get("file"));
    $.ajax({
      url: this.state.url,
      data: formData,
      processData:false,
      contentType: false,
      method: 'POST',
      success: function (data) {
        alert(data);
      }
    });

  }

  createFormData = () => {
    if (this.state.videoInput === undefined) {
      return null;
    }
    console.log(this.state.videoInput)
    var fd = new FormData();
    fd.append('file', this.state.videoInput);
    return fd;
  }

  render() {
    return (
        <div>
            <form encType="multipart/form-data" onSubmit={this.onSubmit}>
          <input id="video-file" name="videoInput" type="file" onChange={this.onVideoInputChange} />
          <input type="submit" value="Submit" />
        </form>
      </div>
    );
  }
}
