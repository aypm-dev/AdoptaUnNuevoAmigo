import { create } from "zustand";

export type CatalogSort = "recientes" | "nombre" | "edad";

type CatalogStore = {
  selectedSpeciesId: string;
  searchQuery: string;
  sortBy: CatalogSort;
  currentPage: number;
  setSpecies: (speciesId: string) => void;
  setSearch: (query: string) => void;
  setSort: (sort: CatalogSort) => void;
  setPage: (page: number) => void;
  resetFilters: () => void;
};

export const useCatalogStore = create<CatalogStore>()((set) => ({
  selectedSpeciesId: "all",
  searchQuery: "",
  sortBy: "recientes",
  currentPage: 1,
  setSpecies: (selectedSpeciesId) => set({ selectedSpeciesId, currentPage: 1 }),
  setSearch: (searchQuery) => set({ searchQuery, currentPage: 1 }),
  setSort: (sortBy) => set({ sortBy, currentPage: 1 }),
  setPage: (currentPage) => set({ currentPage }),
  resetFilters: () =>
    set({
      selectedSpeciesId: "all",
      searchQuery: "",
      sortBy: "recientes",
      currentPage: 1,
    }),
}));
