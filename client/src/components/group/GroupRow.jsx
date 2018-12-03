import React from 'react';
import { Link } from 'react-router-dom';

import { Card, CardBody, CardTitle, CardSubtitle, CardText, Row, Col } from 'reactstrap';

const GroupRow = ({ group }) => (
  <Row className="justify-content-center">
    <Col md="6">
      <Card className="mx-4">
        <CardBody className="p-4">
          <CardTitle>
            <Link to={'/groups/' + group.name}>{group.name} by {group.owner.displayName}</Link>
          </CardTitle>
          <CardSubtitle>
            {group.isOwner && 'You have created this group'}
          </CardSubtitle>
        </CardBody>
      </Card>
    </Col>
  </Row>
);

export default GroupRow;