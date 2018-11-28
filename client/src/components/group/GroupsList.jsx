import React from 'react';

import { Container } from 'reactstrap'
import GroupRow from './GroupRow';

const GroupsList = ({ groups }) => {
  return (
    <Container>
      {groups.map((group, index) =>
        <GroupRow group={group} key={index} />
      )}
    </Container>);
}

export default GroupsList;