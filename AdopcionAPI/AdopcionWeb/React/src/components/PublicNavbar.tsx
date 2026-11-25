import { Button, Chip, Navbar, NavbarBrand, NavbarContent, NavbarItem } from "@heroui/react";
import { PawIcon } from "@/components/PawIcon";

interface PublicNavbarProps {
  availableCount?: number;
  onLoginClick: () => void;
}

export function PublicNavbar({ availableCount, onLoginClick }: PublicNavbarProps) {
  return (
    <Navbar
      maxWidth="xl"
      isBordered
      className="bg-[#FAF8F5]/90 backdrop-blur-md border-black/5"
    >
      <NavbarBrand>
        <a href="/" className="flex items-center gap-2.5 text-[#11181C] no-underline">
          <PawIcon color="#292D32" size={22} />
          <span className="text-[15px] font-bold tracking-tight">
            Adopta Un Nuevo Amigo
          </span>
        </a>
      </NavbarBrand>

      <NavbarContent justify="end" className="hidden md:flex">
        {typeof availableCount === "number" && (
          <NavbarItem>
            <Chip color="success" variant="flat" size="sm">
              {availableCount} mascotas disponibles
            </Chip>
          </NavbarItem>
        )}
        <NavbarItem>
          <Button
            size="sm"
            radius="full"
            variant="bordered"
            className="border-default-300 bg-white font-semibold text-[#11181C]"
            onPress={onLoginClick}
          >
            Iniciar sesión
          </Button>
        </NavbarItem>
      </NavbarContent>
    </Navbar>
  );
}
