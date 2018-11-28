import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getUsers } from '../../actions/userActions';

import { Container, Button, Row, Col, Card, CardBody, InputGroup, Input } from 'reactstrap'
import UserRow from './UserRow'

const initalFilter = {
  pattern: '',
  sortAscending: true,
  offset: 0,
  limit: 0
};

class UsersList extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      filter: Object.assign({}, initalFilter)
    };

    this.patternChange = this.patternChange.bind(this);
  }

  componentDidMount() {
    this.props.getUsers();
  }

  patternChange(event) {
    const pattern = event.target.value;

    let filter = Object.assign({}, this.state);

    if (pattern != filter.pattern) {
      filter = Object.assign({}, initalFilter);
      filter.pattern = pattern;

      this.props.getUsers(filter.pattern, filter.sortAscending, filter.offset, filter.limit);

      this.setState({ filter });
    }
  }

  render() {
    return (<Container>
      <Row className="justify-content-center">
        <Col md="6">
          <Card className="mx-4">
            <CardBody className="p-4">
              <InputGroup className="mb-3">
                <Input onChange={this.patternChange}
                  value={this.state.filter.pattern}
                  name="username"
                  type="text" />
              </InputGroup>
            </CardBody>
          </Card>
        </Col>
      </Row>

      {this.props.users.map((user, index) =>
        <UserRow user={user} key={index} />
      )}
    </Container>);
  }
}

const mapStateToProps = (state, ownProps) => {
  return { users: state.user.users || [] };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getUsers }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(UsersList);