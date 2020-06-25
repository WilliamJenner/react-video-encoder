import React, { Component } from 'react';
import { Button } from 'reactstrap';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
            <Button onClick={() => {
                fetch("/VideoEncoder/StartWrite", {
                    method: "POST",
                }).then(res => {
                    console.log("Response ", res);
                })
            }}>click to start </Button>
      </div>
    );
  }
}
