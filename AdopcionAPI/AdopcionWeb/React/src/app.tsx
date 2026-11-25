import "./app.css";
import { createRoot } from "react-dom/client";
import { createInertiaApp } from "@inertiajs/react";
import { HeroUIProvider } from "@heroui/react";

createInertiaApp({
  title: (title) =>
    title ? `${title} - Adopta Un Nuevo Amigo` : "Adopta Un Nuevo Amigo",
  resolve: (name) => {
    const pages = import.meta.glob("./Pages/**/*.tsx", { eager: true });
    const page = pages[`./Pages/${name}.tsx`];
    if (!page) {
      throw new Error(`Inertia page not found: ./Pages/${name}.tsx`);
    }
    return page;
  },
  setup({ el, App, props }) {
    createRoot(el).render(
      <HeroUIProvider>
        <App {...props} />
      </HeroUIProvider>
    );
  },
});
