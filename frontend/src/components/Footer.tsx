import React from 'react';

const Footer: React.FC = () => {
  return (
    <footer className="">
      <p>&copy; {new Date().getFullYear()} Strous-troupe team. All rights reserved.</p>
    </footer>
  );
};

export default Footer;