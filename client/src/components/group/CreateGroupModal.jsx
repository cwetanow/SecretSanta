import React from 'react';
import Modal from 'react-bootstrap4-modal';
import { Button, FormGroup, Input, Form, Col, Label, Row } from 'reactstrap';
import { bindActionCreators } from 'redux';

export class CreateGroupModal extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      group: {
        groupName: ''
      }
    };

    this.onGroupNameChange = this.onGroupNameChange.bind(this);
  }

  onGroupNameChange(newGroupName) {
    const group = Object.assign({}, this.state.group);
    group.groupName = newGroupName;

    this.setState({ group });
  }

  render() {
    return (<Modal visible={true} onClickBackdrop={this.props.closeModal} dialogClassName="modal-lg">
      <Form onSubmit={ev => ev.preventDefault()}>
        <FormGroup>
          <Row>
            <Col md="6">
              <Label>Name</Label>
              <Input onChange={(ev) => this.onGroupNameChange(ev.target.value)}
                value={this.state.group.groupName} type="text" />
            </Col>
          </Row>
        </FormGroup>

        <FormGroup>
          <Button type="submit" size="xl" color="primary" onClick={() => this.props.createGroup(this.state.group)} disabled={!this.state.group.groupName}><i className="fa fa-dot-circle-o"></i> Save</Button>
          <Button size="xl" color="primary" className="float-right" onClick={this.props.closeModal}><i className="fa fa-dot-circle-o"></i> Close</Button>
        </FormGroup>
      </Form>
    </Modal >);
  }
}

export default CreateGroupModal;