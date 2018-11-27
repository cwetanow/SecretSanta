import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { register } from '../../actions/authActions';
import { Redirect } from 'react-router-dom';
import { Container, Row, Col, Card, CardBody, Button, Input, InputGroup, InputGroupAddon, InputGroupText } from 'reactstrap';

class Register extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      user: {
        displayName: '',
        username: '',
        email: '',
        password: ''
      }
    }

    this.onChange = this.onChange.bind(this);
    this.onRegisterClick = this.onRegisterClick.bind(this);
  }

  onRegisterClick() {
    this.props.register(this.state.user);
  }

  onChange(event) {
    const user = Object.assign({}, this.state.user);

    const propertyName = event.target.name;
    user[propertyName] = event.target.value;

    this.setState({ user });
  }

  render() {
    if (this.props.isAuthenticated) {
      return <Redirect to="/" />
    }

    return (
      <Container>
        <Row className="justify-content-center">
          <Col md="6">
            <Card className="mx-4">
              <CardBody className="p-4">
                <h1>Register</h1>
                <p className="text-muted">Create your account</p>
                <InputGroup className="mb-3">
                  <Input onChange={this.onChange}
                    value={this.state.user.displayName}
                    name="displayName"
                    placeholder="Enter your Name" type="text" />
                </InputGroup>

                <InputGroup className="mb-3">
                  <Input onChange={this.onChange}
                    value={this.state.user.email}
                    name="email"
                    type="text" placeholder="Email" />
                </InputGroup>

                <InputGroup className="mb-3">
                  <Input onChange={this.onChange}
                    value={this.state.user.username}
                    name="username"
                    type="text" placeholder="username" />
                </InputGroup>

                <InputGroup className="mb-3">
                  <Input onChange={this.onChange}
                    value={this.state.user.password}
                    name="password"
                    type="password" placeholder="Password" />
                </InputGroup>

                <Button onClick={this.onRegisterClick} color="success" block>Register</Button>
              </CardBody>
            </Card>
          </Col>
        </Row>
      </Container>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return { isAuthenticated: state.auth && state.auth.isAuthenticated };
}


const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ register }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(Register);