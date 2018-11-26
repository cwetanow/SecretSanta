import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Route from 'react-router-dom/Route';
import { withRouter } from 'react-router';

import { Container, Jumbotron } from 'reactstrap';
import Navbar from './common/Navbar';

class App extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <Container fluid>
        <Navbar></Navbar>

      </Container>
    );
  }
}

App.propTypes = {
  children: PropTypes.object,
}

function mapStateToProps(state, ownProps) {
  return {};
}

export default withRouter(connect(mapStateToProps)(App));
