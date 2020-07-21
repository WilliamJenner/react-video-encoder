import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Constants } from "../utils/constants";


export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
        <div>
            <form enctype="multipart/form-data" action={`${Constants.apiUrl}${Constants.videoControllerUrl}${Constants.setVideoUrl}`} method="post">
                <input id="video-file" name="videoInput" type="file" />
                <input type="submit" value="Submit" />
            </form>
      </div>
    );
  }
}
