import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { logout } from '../../actions/authActions';
import { Redirect } from 'react-router-dom';
import { Container, Row, Col, Card, CardBody, Button, Input, InputGroup, InputGroupAddon, InputGroupText } from 'reactstrap';

class Logout extends Component {
  render() {
    this.props.logout();

    return (
      <Redirect to="/" />
    );
  }
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ logout }, dispatch)
};

export default connect(null, mapDispatchToProps)(Logout);