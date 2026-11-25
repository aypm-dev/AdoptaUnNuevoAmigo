import { Head, router } from "@inertiajs/react";
import { Button, Card, CardBody, CardHeader, Chip, Divider } from "@heroui/react";

interface EspecieDto {
  id: number;
  nombre: string;
  descripcion: string;
  activo: boolean;
  fechaRegistro: string;
}

interface HomeIndexProps {
  especies: EspecieDto[];
  apiError?: string | null;
}

export default function Index({ especies, apiError }: HomeIndexProps) {
  return (
    <>
      <Head title="Inicio" />
      <main className="mx-auto max-w-4xl px-6 py-10">
        <div className="mb-8 flex items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-bold tracking-tight">
              Adopta Un Nuevo Amigo
            </h1>
            <p className="mt-1 text-default-500">
              MVP conectado a AdopcionAPI (ASP.NET Core + Inertia + React + HeroUI)
            </p>
          </div>
          <Button
            color="warning"
            variant="flat"
            onPress={() => router.reload({ only: ["especies", "apiError"] })}
          >
            Recargar API
          </Button>
        </div>

        {apiError && (
          <Card className="mb-6 border border-danger-200 bg-danger-50">
            <CardBody>
              <p className="font-semibold text-danger-700">
                No se pudo conectar con AdopcionAPI
              </p>
              <p className="mt-1 text-sm text-danger-600">{apiError}</p>
            </CardBody>
          </Card>
        )}

        <Card>
          <CardHeader className="flex items-center justify-between px-6 py-4">
            <h2 className="text-lg font-semibold">
              Especies registradas (GET /api/especies)
            </h2>
            <Chip color="primary" variant="flat" size="sm">
              {especies.length} encontradas
            </Chip>
          </CardHeader>
          <Divider />
          <CardBody className="p-6">
            {especies.length === 0 ? (
              <p className="text-default-500">
                No hay especies registradas todavía en la base de datos.
              </p>
            ) : (
              <div className="grid gap-4 sm:grid-cols-2">
                {especies.map((especie) => (
                  <Card key={especie.id} shadow="sm" className="border border-default-200">
                    <CardBody className="gap-2">
                      <div className="flex items-center justify-between">
                        <span className="text-base font-semibold">
                          {especie.nombre}
                        </span>
                        <Chip
                          color={especie.activo ? "success" : "default"}
                          size="sm"
                          variant="flat"
                        >
                          {especie.activo ? "Activa" : "Inactiva"}
                        </Chip>
                      </div>
                      <p className="text-sm text-default-500">
                        {especie.descripcion}
                      </p>
                    </CardBody>
                  </Card>
                ))}
              </div>
            )}
          </CardBody>
        </Card>
      </main>
    </>
  );
}
