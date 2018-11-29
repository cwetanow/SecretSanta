import React from 'react';
import { Link } from 'react-router-dom';

import { Card, CardBody, CardTitle, CardSubtitle, CardText, Row, Col, Button } from 'reactstrap';

const Invite = ({ invite, answerInvite }) => (
  <Row className="justify-content-center">
    <Col md="6">
      <Card className="mx-4">
        <CardBody className="p-4">
          <CardTitle>
            {invite.groupName}
            <Button onClick={() => answerInvite(true, invite.groupName)} color="success" block>Accept</Button>
            <Button onClick={() => answerInvite(false, invite.groupName)} color="success" block>Reject</Button>
          </CardTitle>
        </CardBody>
      </Card>
    </Col>
  </Row>
);

export default Invite;