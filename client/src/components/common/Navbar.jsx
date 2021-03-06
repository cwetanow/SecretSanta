import React from 'react';
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
  UncontrolledDropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem
} from 'reactstrap';
import { Link } from 'react-router-dom';

export default class NavBar extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      isOpen: false
    };

    this.toggle = this.toggle.bind(this);
    this.renderNavLink = this.renderNavLink.bind(this);
  }

  toggle() {
    this.setState({
      isOpen: !this.state.isOpen
    });
  }


  renderNavLink(label, to) {
    return (<NavLink tag={Link} to={to}> {label} </NavLink>);
  };

  render() {
    return (
      <div>
        <Navbar color="light" light expand="md">
          <NavbarBrand href="/">SecretSanta</NavbarBrand>
          <NavbarToggler onClick={this.toggle} />
          <Collapse isOpen={this.state.isOpen} navbar>
            <Nav className="ml-auto" navbar>
              {!this.props.isAuthenticated &&
                <NavItem>
                  {this.renderNavLink('Login', '/login')}
                </NavItem>}
              {!this.props.isAuthenticated &&
                <NavItem>
                  {this.renderNavLink('Register', '/register')}
                </NavItem>
              }

              {this.props.isAuthenticated &&
                <NavItem>
                  {this.renderNavLink('Invites', '/invites')}
                </NavItem>}
              {this.props.isAuthenticated &&
                <NavItem>
                  {this.renderNavLink('Users', '/users')}
                </NavItem>}
              {this.props.isAuthenticated &&
                <NavItem>
                  {this.renderNavLink('Logout', '/logout')}
                </NavItem>}
            </Nav>
          </Collapse>
        </Navbar>
      </div>
    );
  }
}