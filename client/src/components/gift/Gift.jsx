import React from 'react';
import { Link } from 'react-router-dom';

import { Card, CardBody, CardTitle, CardSubtitle, CardText, Row, Col } from 'reactstrap';

const Gift = ({ gift }) => (
  <Row className="justify-content-center">
    <Col md="6">
      <Card className="mx-4">
        <CardBody className="p-4">
          <CardTitle>
            You are giving to <Link to={'/users/' + gift.receiver}>{gift.receiver}</Link>
          </CardTitle>
        </CardBody>
      </Card>
    </Col>
  </Row>
);

export default Gift;