import React from 'react';

interface FooterProps {
  year: number;
}

const Footer: React.FC<FooterProps> = ({ year }) => {
  return (
    <footer className="">
      <p>&copy; {year} Strous-troupe team. All rights reserved.</p>
    </footer>
  );
};

export default Footer;