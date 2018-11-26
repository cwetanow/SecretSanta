import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { login } from '../../actions/authActions';
import { Redirect } from 'react-router-dom';
import { Container, Row, Col, Card, CardBody, Button, Input, InputGroup, InputGroupAddon, InputGroupText } from 'reactstrap';

class Login extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      user: {
        username: '',
        password: ''
      }
    }

    this.onChange = this.onChange.bind(this);
    this.onLoginClick = this.onLoginClick.bind(this);
  }

  onLoginClick() {
    this.props.login(this.state.user);
  }

  onChange(event) {
    const user = Object.assign({}, this.state.user);

    const propertyName = event.target.name;
    user[propertyName] = event.target.value;

    this.setState({ user });
  }

  render() {
    return (
      <Container>
        <Row className="justify-content-center">
          <Col md="6">
            <Card className="mx-4">
              <CardBody className="p-4">
                <h1>Login</h1>

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

                <Button onClick={this.onLoginClick} color="success" block>Login</Button>
              </CardBody>
            </Card>
          </Col>
        </Row>
      </Container>
    );
  }
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ login }, dispatch)
};

export default connect(null, mapDispatchToProps)(Login);