import React from 'react';
import { Button, FormGroup, Input, Form, Col, Label, Row, Card, CardBody, CardTitle, CardText, CardSubtitle } from 'reactstrap';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getMessages } from '../../actions/messageActions';
import socketIOClient from 'socket.io-client'

export class Chat extends React.Component {
  constructor(props, context) {
    super(props, context);
    const message = { room: props.room, user: props.user, content: '', time: '' };

    const socket = this.initSocket();

    this.state = {
      message,
      socket,
      messages: props.messages || []
    };

    this.onContentChange = this.onContentChange.bind(this);
    this.sendMessage = this.sendMessage.bind(this);

    props.getMessages(props.room);
  }

  componentWillReceiveProps(nextProps, ownProps) {
    if (nextProps.messages) {
      this.setState({ messages: nextProps.messages });
    }
  }

  initSocket() {
    const socket = socketIOClient('http://localhost:1235');

    socket.on('chat message', (message) => {
      if (message) {
        const messages = [...this.state.messages];
        messages.push(message);

        this.setState({ messages });
      }
    });

    return socket;
  }

  onContentChange(value) {
    let message = Object.assign({}, this.state.message);
    message.content = value;

    this.setState({ message });
  }

  sendMessage() {
    const message = Object.assign({}, this.state.message);
    message.time = new Date();

    this.state.socket.emit('chat message', message);

    this.onContentChange('');
  }

  getTimeText(time) {
    let date = new Date(time);
    return date.toLocaleString('bg-BG');
  }

  renderNoMessagesText() {
    return <div className="empty-message"><em>Discussion will appear here.</em></div>
  }

  render() {
    return (
      <div>
        {(!this.state.messages || !this.state.messages.length) && this.renderNoMessagesText()}
        {this.state.messages && !!this.state.messages.length &&
          this.state.messages
            .map((message, index) => (<Card key={index}>
              <CardBody>
                <CardTitle>{message.user}</CardTitle>
                <CardSubtitle>{this.getTimeText(message.time)}</CardSubtitle>
                <CardText>{message.content}</CardText>
              </CardBody>
            </Card>))}

        {this.props.isActive && <Form onSubmit={(e) => { e.preventDefault() }}>
          <FormGroup>
            <Row>
              <Col md="8">
                <Label>Discussion</Label>
                <Input onChange={(e) => this.onContentChange(e.target.value)}
                  value={this.state.message.content}
                  type="textarea"
                  placeholder="Write you instant message here"
                  cols="5"
                  rows="5" />
              </Col>
            </Row>
          </FormGroup>

          <FormGroup>
            <Button type="submit" size="xl" color="primary" onClick={this.sendMessage}><i className="fa fa-dot-circle-o"></i> Submit</Button>
          </FormGroup>
        </Form>}
      </div>)
  }
}

const mapStateToProps = (state, ownProps) => {
  return {
    ...ownProps,
    messages: state.message.messages
  };
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getMessages }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(Chat);