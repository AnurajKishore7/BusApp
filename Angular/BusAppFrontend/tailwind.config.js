/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'bus-red': '#E63946',  
        'bus-white': '#FFFFFF', 
        'bus-grey': '#F1F1F1',
      },
    },
  },
  plugins: [],
}