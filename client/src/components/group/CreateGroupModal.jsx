import React from 'react';
import Modal from 'react-bootstrap4-modal';
import { Button, FormGroup, Input, Form, Col, Label, Row } from 'reactstrap';
import { getUsers } from '../../actions/userActions';
import { bindActionCreators } from 'redux';

export class CreateGroupModal extends React.Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      group: {
        groupName: ''
      }
    };

    this.createGroup = this.createGroup.bind(this);
  }

  createGroup() {
  }

  render() {
    return (<Modal visible={true} onClickBackdrop={this.props.closeModal} dialogClassName="modal-lg">
      <Form onSubmit={ev => ev.preventDefault()}>
        <FormGroup>
          <Row>
            <Col md="6">
              <Label>Name</Label>
              <Input onChange={this.onInputChange}
                value={this.state.group.groupName} type="text" />
            </Col>
          </Row>
        </FormGroup>

        <FormGroup>
          <Button type="submit" size="xl" color="primary" onClick={this.save}><i className="fa fa-dot-circle-o"></i> Save</Button>
          <Button size="xl" color="primary" className="float-right" onClick={this.props.closeModal}><i className="fa fa-dot-circle-o"></i> Close</Button>
        </FormGroup>
      </Form>
    </Modal >);
  }
}

const mapDispatchToProps = (dispatch) => {
  return bindActionCreators({ getUsers }, dispatch)
};

export default connect(mapStateToProps, mapDispatchToProps)(CreateGroupModal);