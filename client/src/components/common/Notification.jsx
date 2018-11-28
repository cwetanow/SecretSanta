import React from 'react'
import { connect } from 'react-redux'
import toastr from 'toastr';

const Notification = (props) => {
  if (props && props.notification) {
    if (props.notification.error) {
      toastr.error(props.notification.error.message);
    }

    if (props.notification.success) {
      toastr.success(props.notification.success.message);
    }
  }

  return (
    <div className="hidden"></div>
  );
}

const mapStateToProps = state => {
  return {
    notification: state.notification
  };
}

export default connect(mapStateToProps)(Notification)