/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'bus-red': '#E63946',  // Primary red
        'bus-white': '#FFFFFF', // Base white
        'bus-grey': '#F1F1F1',  // Neutral grey
      },
    },
  },
  plugins: [],
}