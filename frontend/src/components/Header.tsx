import React from 'react';

interface HeaderProps {
  title: string;
}

const Header: React.FC<HeaderProps> = ({ title }) => {
  return (
    <header className="">
      <h1 className="">{title}</h1>
    </header>
  );
};

export default Header;