import React from 'react';
import { Link } from 'react-router-dom';

import { Card, CardBody, CardTitle, CardSubtitle, CardText } from 'reactstrap';

const UserRow = ({ user }) => (
  (<Card>
    <CardBody>
      <CardTitle>
        <Link to={'/users/' + user.username}>{user.username}</Link>
      </CardTitle>
    </CardBody>
  </Card>)
);

export default UserRow;