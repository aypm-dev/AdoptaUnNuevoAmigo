import { Button, Input, Select, SelectItem } from "@heroui/react";
import { PawIcon } from "@/components/PawIcon";
import { useCatalogStore, type CatalogSort } from "@/stores/useCatalogStore";
import type { EspecieDto } from "@/types/models";

interface SearchFilterBarProps {
  especies: EspecieDto[];
  onLoginClick: () => void;
}

function pluralizeSpecies(nombre: string): string {
  const lower = nombre.toLowerCase();
  if (lower.endsWith("s")) return nombre;
  return `${nombre}s`;
}

export function SearchFilterBar({ especies, onLoginClick }: SearchFilterBarProps) {
  const {
    selectedSpeciesId,
    searchQuery,
    sortBy,
    setSpecies,
    setSearch,
    setSort,
  } = useCatalogStore();

  const speciesItems = [
    { key: "all", label: "Mostrando Todas" },
    ...especies.map((e) => ({
      key: String(e.id),
      label: `Mostrando ${pluralizeSpecies(e.nombre)}`,
    })),
  ];

  return (
    <section className="mt-6 rounded-2xl bg-[#F59E0B] p-3 shadow-[0_12px_26px_-6px_rgba(245,158,11,0.42),0_6px_14px_-4px_rgba(0,0,0,0.16)]">
      <div className="flex flex-col gap-2.5 md:flex-row md:items-center md:gap-3">
        <div className="flex w-full items-center gap-2.5 md:w-auto md:shrink-0">
          <div className="shrink-0 pl-1">
            <PawIcon color="#ECEBEB" size={26} />
          </div>

          <Select
            aria-label="Filtrar por especie"
            selectedKeys={[selectedSpeciesId]}
            onSelectionChange={(keys) => {
              const first = Array.from(keys)[0];
              setSpecies(first ? String(first) : "all");
            }}
            variant="flat"
            radius="full"
            size="md"
            className="flex-1 md:w-52 md:flex-none"
            classNames={{
              trigger: "bg-[#F4F4F5] shadow-sm data-[hover=true]:bg-white",
              value: "text-sm font-medium text-[#11181C]",
            }}
          >
            {speciesItems.map((item) => (
              <SelectItem key={item.key}>{item.label}</SelectItem>
            ))}
          </Select>

          <Button
            isIconOnly
            radius="full"
            size="md"
            variant="flat"
            aria-label="Iniciar sesión"
            title="Iniciar sesión (Personal del refugio)"
            onPress={onLoginClick}
            className="shrink-0 bg-[#F4F4F5] text-[#F59E0B] shadow-sm md:hidden"
          >
            <svg
              width="20"
              height="20"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
              aria-hidden="true"
            >
              <circle cx="12" cy="8" r="4" fill="currentColor" />
              <path
                d="M4 20C4 16.6863 7.58172 14 12 14C16.4183 14 20 16.6863 20 20"
                stroke="currentColor"
                strokeWidth="2.2"
                strokeLinecap="round"
              />
            </svg>
          </Button>
        </div>

        <Input
          aria-label="Buscar por nombre o raza"
          value={searchQuery}
          onValueChange={setSearch}
          placeholder="Buscar por nombre o raza"
          variant="flat"
          radius="full"
          size="md"
          isClearable
          onClear={() => setSearch("")}
          className="flex-1"
          classNames={{
            inputWrapper:
              "bg-[#F4F4F5] shadow-sm data-[hover=true]:bg-white group-data-[focus=true]:bg-white",
            input: "text-sm text-[#11181C] placeholder:text-[#52525B]",
          }}
          endContent={
            <svg
              width="16"
              height="16"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
              className="pointer-events-none shrink-0 text-[#A1A1AA]"
              aria-hidden="true"
            >
              <circle cx="11" cy="11" r="7" stroke="currentColor" strokeWidth="2.2" />
              <path d="M20 20L16.5 16.5" stroke="currentColor" strokeWidth="2.2" strokeLinecap="round" />
            </svg>
          }
        />

        <Select
          aria-label="Ordenar mascotas"
          selectedKeys={[sortBy]}
          onSelectionChange={(keys) => {
            const first = Array.from(keys)[0];
            if (first) setSort(String(first) as CatalogSort);
          }}
          variant="flat"
          radius="full"
          size="md"
          className="w-full md:w-48 md:shrink-0"
          classNames={{
            trigger: "bg-[#F4F4F5] shadow-sm data-[hover=true]:bg-white",
            value: "text-sm font-medium text-[#52525B]",
          }}
        >
          <SelectItem key="recientes">Orden: Más recientes</SelectItem>
          <SelectItem key="nombre">Orden: Nombre A–Z</SelectItem>
          <SelectItem key="edad">Orden: Menor edad</SelectItem>
        </Select>
      </div>
    </section>
  );
}
