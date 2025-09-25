import React from 'react';
import Header from './components/Header';
import Footer from './components/Footer';
import './index.css';

const App: React.FC = () => {
  
  const handleClick = () => {
    alert('Пока нет, но скоро будет!');
  };
  
  return (
    <div className="">
      <Header title="Ask Ostrich website" />
      <main className="">
        <img
          src="/src/assets/placeholder.png"
          alt="Placeholder Image"
          className=""
        />
        <br></br>
        <button
            onClick={handleClick}
            className="btn-max-width"
          >
            Хочу!
          </button>
      </main>
      <Footer year={2025} />
    </div>
  );
};

export default App;